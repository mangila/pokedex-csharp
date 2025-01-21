export type Loglevel = 'debug' | 'info' | 'warn' | 'error';

export interface LokiLogRequest {
    loglevel: Loglevel;
    message: string;
    data: unknown
}