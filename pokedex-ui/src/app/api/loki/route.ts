import {NextRequest, NextResponse} from "next/server";
import {loki} from "../../../loki"
import {LokiLogRequest} from "@/types";

// Log request server side to Loki
export async function POST(request: NextRequest) {
    const body: LokiLogRequest = await request.json()
    loki.log(body.loglevel, body.message, body.data);
    return NextResponse.json({}, {status: 200});
}