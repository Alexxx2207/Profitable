import { request } from "../../utils/fetch/request";
import { WEB_API_BASE_URL, JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE } from '../../common/config';

export const loginUser = async (email, password) => {
    let response = await request.post(`${WEB_API_BASE_URL}/users/login`, {
        email,
        password
    });

    if (response.status === 401) {
        throw new Error(await response.text());
    }
    
    return await response.json();
}

export const registerUser = async (email, firstName, lastName, password) => {
    let response = await request.post(`${WEB_API_BASE_URL}/users/register`, {
        email,
        password,
        firstName, 
        lastName,
    });

    if (response.status === 401) {
        throw new Error(await response.text());
    }
    return await response.json();
}

export const editUser = async (email, firstName, lastName, description, jwt) => {
    let response = await request.patch(`${WEB_API_BASE_URL}/users/user/edit`, {
        email,
        firstName,
        lastName,
        description
    }, {
        'Authorization': 'Bearer ' + jwt
    });

    if (response.status === 401) {
        throw new Error(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE);
    }
    return await response.json();
}

export const getUserDataByJWT = async (jwt) => {
    let response = await request.get(`${WEB_API_BASE_URL}/users/user`, null, {
        'Authorization': 'Bearer ' + jwt
    });

    if (response.status === 401) {
        throw new Error(await response.text());
    }

    return await response.json();
}

export const getUserDataByEmail = async (email) => {
    let response = await request.get(`${WEB_API_BASE_URL}/users/user/${email}`);

    return await response.json();
}

export const getUserEmailFromJWT = async (jwt) => {
    let response = await request.get(`${WEB_API_BASE_URL}/users/user/email`, null, {
        'Authorization': 'Bearer ' + jwt
    });

    if (response.status === 401) {
        throw new Error(await response.text());
    }

    return await response.text();
}