import winston from "winston";
import LokiTransport from "winston-loki";

// Loki transport configuration
const loki = winston.createLogger({
    transports: [
        new LokiTransport({
            host: "http://localhost:3100",
            labels: {app: "pokedex-ui"},
            json: true,
            format: winston.format.json(),
            onConnectionError: (err) => {
                console.error("Loki connection error:", err);
            },
        }),
    ],
});

export default loki;