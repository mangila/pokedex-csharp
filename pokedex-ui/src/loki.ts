import winston from "winston";
import LokiTransport from "winston-loki";
import * as process from "node:process";

// Loki transport configuration
export const loki = winston.createLogger({
    transports: [
        new LokiTransport({
            host: "http://localhost:3100",
            labels: {app: "pokedex-ui", env: process.env.NODE_ENV},
            json: true,
            format: winston.format.json(),
            onConnectionError: (err) => {
                console.error("Loki connection error:", err);
            },
        }),
    ],
});