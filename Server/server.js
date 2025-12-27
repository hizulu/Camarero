require('dotenv').config();
const express = require('express');
const { MongoClient } = require('mongodb');
const cors = require('cors');

const app = express();
app.use(cors());
app.use(express.json());

const uri = `mongodb+srv://${process.env.DB_USER}:${process.env.DB_PASS}@${process.env.DB_CLUSTER}/${process.env.DB_NAME}?retryWrites=true&w=majority`;
const client = new MongoClient(uri);

async function startServer() {
    try {
        await client.connect();
        console.log("Conectado a MongoDB");
        const db = client.db(process.env.DB_NAME);
        const collection = db.collection("Player");

        // Guardar o actualizar datos del jugador
        app.post('/api/players', async (req, res) => {
    try {
        console.log("Cuerpo recibido:", req.body); // <--- imprime lo que llega desde Unity

        const { username, best_score, games_played, last_game_date } = req.body;

        if (!username || best_score == null || games_played == null || !last_game_date) {
            throw new Error("Faltan campos obligatorios en el cuerpo");
        }

        const existing = await collection.findOne({ username });

        if (existing) {
            await collection.updateOne(
                { username },
                { 
                    $set: { last_game_date: new Date(last_game_date) },
                    $max: { best_score },
                    $inc: { games_played: 1 }
                }
            );
        } else {
            await collection.insertOne({
                username,
                best_score,
                games_played,
                last_game_date: new Date(last_game_date)
            });
        }

        res.send({ success: true });
    } catch(e) {
        console.error("Error en POST /api/players:", e); // <-- imprime el error exacto
        res.status(500).send({ error: e.message });
    }
});

        // Obtener ranking global
        app.get('/api/players', async (req, res) => {
            try {
                const players = await collection.find({})
                    .sort({ best_score: -1 }) // De mayor a menor
                    .toArray();
                res.send(players);
            } catch(e) {
                res.status(500).send({ error: e.message });
            }
        });

        app.listen(3000, () => console.log('Servidor corriendo en http://localhost:3000'));
    } catch(e) {
        console.error(e);
    }
}

startServer();