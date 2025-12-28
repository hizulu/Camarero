require('dotenv').config();
const express = require('express');
const { MongoClient } = require('mongodb');
const cors = require('cors');

const app = express();
app.use(cors());
app.use(express.json());

// URI de MongoDB usando variables de entorno
const uri = `mongodb+srv://${process.env.DB_USER}:${process.env.DB_PASS}@${process.env.DB_CLUSTER}/${process.env.DB_NAME}?retryWrites=true&w=majority`;
const client = new MongoClient(uri);

// Obtener la colección
async function getCollection() {
    if (!client.topology || !client.topology.isConnected()) {
        await client.connect();
    }
    const db = client.db(process.env.DB_NAME || "Camarero");
    return db.collection("Player");
}

// Ruta raíz para pruebas
app.get('/', (req, res) => {
    res.send('Backend Camarero corriendo!');
});

// Guardar o actualizar datos del jugador
app.post('/api/players', async (req, res) => {
    try {
        const collection = await getCollection();
        const { username, best_score, games_played, last_game_date } = req.body;

        if (!username) return res.status(400).json({ error: "Username requerido" });

        const existingPlayer = await collection.findOne({ username });

        if (existingPlayer) {
            // Actualiza solo si el nuevo score es mejor
            const updatedBestScore = Math.max(existingPlayer.best_score, best_score);

            await collection.updateOne(
                { username },
                {
                    $set: {
                        best_score: updatedBestScore,
                        last_game_date: last_game_date
                    },
                    $inc: { games_played: games_played }
                }
            );

            res.json({ success: true, message: "Jugador actualizado" });
        } else {
            // Crea nuevo jugador
            await collection.insertOne({ username, best_score, games_played, last_game_date });
            res.json({ success: true, message: "Jugador creado" });
        }
    } catch (e) {
        console.error(e);
        res.status(500).json({ error: e.message });
    }
});

// Obtener ranking global
app.get('/api/players', async (req, res) => {
    try {
        const collection = await getCollection();
        const players = await collection.find({}).toArray();

        // Ordenar por mejor puntuación
        players.sort((a, b) => b.best_score - a.best_score);

        res.json(players);
    } catch (e) {
        console.error(e);
        res.status(500).json({ error: e.message });
    }
});

// Puerto dinámico para hosting (Render, Railway, etc.)
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Servidor corriendo en puerto ${PORT}`));
