import { WEB_API_BASE_URL } from "../../common/config";
import { request } from "../../utils/fetch/request";

export const getPositionsRecordsOrderByOptions = async () => {
    var response = await request.get(`${WEB_API_BASE_URL}/stockspositions/records/order-options`);

    if (response.status === 400) {
        throw new Error("Invalid request");
    }

    return await response.json();
};

export const getPositionsFromRecord = async (recordId, dateAfter, beforeDate) => {
    var response = await request.get(
        `${WEB_API_BASE_URL}/stockspositions/records/${recordId}/positions?afterDate=${dateAfter}&beforeDate=${beforeDate}`
    );

    if (response.status === 400) {
        throw new Error("Invalid request");
    }

    return await response.json();
};

export const getPositionByGuid = async (positionGuid) => {
    var response = await request.get(`${WEB_API_BASE_URL}/stockspositions/${positionGuid}`);

    if (response.status === 400) {
        throw new Error("Invalid request");
    }

    return await response.json();
};

export const createPosition = async (
    JWT,
    recordId,
    name,
    entryPrice,
    exitPrice,
    quantitySize,
    buyCommission,
    sellCommission
) => {
    var response = await request.post(
        `${WEB_API_BASE_URL}/stockspositions/records/${recordId}/positions`,
        {
            name,
            entryPrice,
            exitPrice,
            quantitySize,
            buyCommission,
            sellCommission,
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
    name,
    entryPrice,
    exitPrice,
    quantitySize,
    buyCommission,
    sellCommission
) => {
    var response = await request.patch(
        `${WEB_API_BASE_URL}/stockspositions/records/${recordId}/positions/${positionGuid}/change`,
        {
            name,
            entryPrice,
            exitPrice,
            quantitySize,
            buyCommission,
            sellCommission,
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
        `${WEB_API_BASE_URL}/stockspositions/records/${recordId}/positions/${positionGuid}/delete`,
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
