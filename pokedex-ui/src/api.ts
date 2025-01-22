import {LokiLogRequest} from "./types";

export const sendLogToLoki = async (request: LokiLogRequest): Promise<boolean> => {
    const response = await fetch('/api/loki', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(request),
    });
    return response.ok;
};