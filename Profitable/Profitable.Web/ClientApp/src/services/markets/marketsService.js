import { WEB_API_BASE_URL } from '../../common/config';
import { request } from '../../utils/fetch/request';

export const getInstrument = (instrument) => {
    return request.get(`${WEB_API_BASE_URL}/markets/${instrument}`)
    .then(response => response.json())
}

export const getAllInstruments = () => {
    return request.get(`${WEB_API_BASE_URL}/markets/`)
        .then(res => res.json());;
}