import { WEB_API_BASE_URL } from '../../common/config';
import { request } from '../../utils/fetch/request';

export const getInstrument = (instrument) => {
    return request.get(`${WEB_API_BASE_URL}/markets/instruments/${instrument}`)
    .then(response => response.json())
}

export const getAllInstruments = () => {
    return request.get(`${WEB_API_BASE_URL}/markets/instruments`)
        .then(res => res.json());
}

export const getAllMarketTypes = () => {
    return request.get(`${WEB_API_BASE_URL}/markets/marketTypes`)
        .then(res => res.json());
}

export const getAllInstrumentsByMarketType = (marketType) => {
    return request.get(`${WEB_API_BASE_URL}/markets/marketTypes/${marketType}/instruments`)
        .then(res => res.json());
}