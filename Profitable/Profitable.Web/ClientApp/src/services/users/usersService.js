import { request } from "../../utils/fetch/request";
import { WEB_API_BASE_URL } from '../../common/config'

export const loginUser = async (userData) => {
    let response = await request.post(`${WEB_API_BASE_URL}/users/login`, userData);

    if (response.status == 401) {
        throw new Error(await response.text());
    }
    
    return await response.json();
}

export const registerUser = async (userData) => {
    let response = await request.post(`${WEB_API_BASE_URL}/users/register`, userData);

    if (response.status == 401) {
        throw new Error(await response.text());
    }
    return await response.json();
}

export const getUserData = async (jwt) => {
    let response = await request.get(`${WEB_API_BASE_URL}/users/user`, null, {
        'Authorization': 'Bearer ' + jwt
    });

    if (response.status == 401) {
        throw new Error(await response.text());
    }

    return await response.json();
}