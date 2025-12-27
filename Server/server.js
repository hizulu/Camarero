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

        const db = client.db("Camarero");          // Base de datos correcta
        const collection = db.collection("Player"); // Colección correcta

        app.post('/api/players', async (req, res) => {
            try {
                await collection.insertOne({
                    username: req.body.username,
                    best_score: req.body.best_score,
                    games_played: req.body.games_played,
                    last_game_date: new Date() // Guarda la fecha de la partida
                });
                res.send({ success: true });
            } catch(e) {
                res.status(500).send({ error: e.message });
            }
        });

        app.get('/api/players', async (req, res) => {
            try {
                const players = await collection.find({}).toArray();
                res.send(players);
            } catch(e) {
                res.status(500).send({ error: e.message });
            }
        });

        app.listen(3000, () => console.log('Servidor corriendo en http://localhost:3000'));
    } catch(e) {
        console.error("Error conectando a MongoDB:", e);
    }
}

startServer();
