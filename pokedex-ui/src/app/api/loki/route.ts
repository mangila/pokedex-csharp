import {pushToLoki} from "@shared/api";
import {LokiLogRequest} from "@shared/types";
import {NextRequest, NextResponse} from "next/server";

// Log request server side to Loki
export async function POST(request: NextRequest) {
    const body: LokiLogRequest = await request.json()
    const response = await pushToLoki(body)
    return NextResponse.json({response}, {status: 200});
}