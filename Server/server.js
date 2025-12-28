require('dotenv').config();
const express = require('express');
const { MongoClient } = require('mongodb');
const cors = require('cors');

const app = express();
app.use(cors());
app.use(express.json());

const uri = `mongodb+srv://${process.env.DB_USER}:${process.env.DB_PASS}@${process.env.DB_CLUSTER}/${process.env.DB_NAME}?retryWrites=true&w=majority`;
const client = new MongoClient(uri);

async function getCollection() {
    await client.connect();
    const db = client.db("Camarero");   // Nombre de tu DB
    return db.collection("Player");     // Nombre de tu colección
}

// Guardar o actualizar datos del jugador
app.post('/api/players', async (req, res) => {
    try {
        const collection = await getCollection();
        const { username, best_score, games_played, last_game_date } = req.body;

        if (!username) {
            return res.status(400).json({ error: "Username requerido" });
        }

        // Busca si ya existe el jugador
        const existingPlayer = await collection.findOne({ username });

        if (existingPlayer) {
            // Actualiza solo si el nuevo score es mejor
            const updatedBestScore = Math.max(existingPlayer.best_score, best_score);

            // Actualiza games_played sumando los nuevos
            const updatedGamesPlayed = existingPlayer.games_played + games_played;

            await collection.updateOne(
                { username },
                {
                    $set: {
                        best_score: updatedBestScore,
                        last_game_date: last_game_date
                    },
                    $setOnInsert: { username },
                    $inc: { games_played: games_played } // Otra forma de sumar
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
        res.json(players);
    } catch (e) {
        console.error(e);
        res.status(500).json({ error: e.message });
    }
});

app.listen(3000, () => console.log('Servidor corriendo en http://localhost:3000'));
