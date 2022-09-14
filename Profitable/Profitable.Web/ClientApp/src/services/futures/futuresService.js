import { WEB_API_BASE_URL } from '../../common/config';
import { request } from '../../utils/fetch/request';

export const loadFuturesContracts = () => {
    return request.get(`${WEB_API_BASE_URL}/futures/all`)
    .then(response => response.json());
}

export const CalculateTicks = (entryPrice, exitPrice, contractsCount, tickSize) => {
    let result = (exitPrice - entryPrice) / tickSize * contractsCount;

    return result;
}

export const CalculatePL = (directionBullish, entryPrice, exitPrice, contractsCount, tickSize, tickValue) => {
    const directionMultiplier = directionBullish ? 1 : -1;

    let result = (exitPrice - entryPrice) / tickSize * tickValue * contractsCount * directionMultiplier;

    return result;
}