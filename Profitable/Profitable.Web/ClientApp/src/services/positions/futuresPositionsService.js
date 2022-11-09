import {
    LongDirectionName,
    LongDirectionValue,
    ShortDirectionValue,
    WEB_API_BASE_URL,
} from "../../common/config";
import { request } from "../../utils/fetch/request";

export const getPositionsRecordsOrderByOptions = async () => {
    var response = await request.get(`${WEB_API_BASE_URL}/futurespositions/records/order-options`);

    if (response.status === 400) {
        throw new Error("Invalid request");
    }

    return await response.json();
};

export const getPositionsFromRecord = async (recordId, dateAfter, dateBefore) => {
    var response = await request.get(
        `${WEB_API_BASE_URL}/futurespositions/records/${recordId}/positions?afterDate=${dateAfter}&beforeDate=${dateBefore}`
    );

    if (response.status === 400) {
        throw new Error("Invalid request");
    }

    return await response.json();
};

export const getPositionByGuid = async (positionGuid) => {
    var response = await request.get(`${WEB_API_BASE_URL}/futurespositions/${positionGuid}`);

    if (response.status === 400) {
        throw new Error("Invalid request");
    }

    return await response.json();
};

export const createPosition = async (
    JWT,
    recordId,
    contractName,
    direction,
    entryPrice,
    exitPrice,
    quantity,
    tickSize,
    tickValue
) => {
    var response = await request.post(
        `${WEB_API_BASE_URL}/futurespositions/records/${recordId}/positions`,
        {
            contractName,
            direction:
                direction.localeCompare(LongDirectionName) === 0
                    ? LongDirectionValue
                    : ShortDirectionValue,
            entryPrice,
            exitPrice,
            quantity,
            tickSize,
            tickValue,
        },
        {
            Authorization: "Bearer " + JWT,
        }
    );

    if ([400, 401].includes(response.status)) {
        throw new Error(await response.text());
    }

    return;
};

export const changePosition = async (
    JWT,
    recordId,
    positionGuid,
    contractName,
    direction,
    entryPrice,
    exitPrice,
    quantity,
    tickSize,
    tickValue
) => {
    var response = await request.patch(
        `${WEB_API_BASE_URL}/futurespositions/records/${recordId}/positions/${positionGuid}/change`,
        {
            contractName,
            direction:
                direction.localeCompare(LongDirectionName) === 0
                    ? LongDirectionValue
                    : ShortDirectionValue,
            entryPrice,
            exitPrice,
            quantity,
            tickSize,
            tickValue,
        },
        {
            Authorization: "Bearer " + JWT,
        }
    );
    if ([400, 401].includes(response.status)) {
        throw new Error(await response.text());
    }
    return;
};

export const deletePosition = async (JWT, recordId, positionGuid) => {
    var response = await request.delete(
        `${WEB_API_BASE_URL}/futurespositions/records/${recordId}/positions/${positionGuid}/delete`,
        null,
        {
            Authorization: "Bearer " + JWT,
        }
    );

    if (response.status === 401) {
        throw new Error(response.status);
    }
    if (response.status === 400) {
        throw new Error(await response.text());
    }

    return;
};

export const calculateAcculativePositions = (positions) => {
    positions = positions.map((position) => Number(position));

    const result = [positions[0]];

    for (let i = 1; i < positions.length; i++) {
        result.push(result[i - 1] + positions[i]);
    }

    return result;
};
