import { WEB_API_BASE_URL } from '../../common/config';

export const getInstrument = (instrument) => {
    return fetch(`${WEB_API_BASE_URL}/markets/${instrument}`)
    .then(response => response.json())
}

export const getAllInstruments = () => {
    return fetch(`${WEB_API_BASE_URL}/markets/`)
        .then(res => res.json());;
}