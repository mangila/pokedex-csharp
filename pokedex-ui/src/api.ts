import {LokiLogRequest} from "./types";

export const sendLogToLoki = async (request: LokiLogRequest) => {
    fetch('/api/loki', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(request),
    });
};