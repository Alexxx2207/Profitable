import { request } from "../../utils/fetch/request";
import {
    WEB_API_BASE_URL,
    JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
    INVALID_OLD_PASSWORD_PROVIDED_ERROR_MESSAGE,
} from "../../common/config";

import { sha512 } from 'crypto-hash';

export const loginUser = async (email, password) => {
    let response = await request.post(`${WEB_API_BASE_URL}/users/login`, {
        email,
        password: await sha512(password),
    });

    if (!response.ok) {
        let errorMessage = await response.text();
        if (errorMessage === "Sequence contains no elements.") {
            errorMessage =
                "We haven't found you.\\nCheck the provided email and password for misspellings.";
        }
        throw new Error(errorMessage);
    }

    return await response.json();
};

export const registerUser = async (email, firstName, lastName, password) => {
    let response = await request.post(`${WEB_API_BASE_URL}/users/register`, {
        email,
        password: await sha512(password),
        firstName,
        lastName,
    });

    if (!response.ok) {
        throw new Error(await response.text());
    }
    return await response.json();
};

export const editGeneralUserData = async (email, firstName, lastName, description, jwt) => {
    let response = await request.patch(
        `${WEB_API_BASE_URL}/users/user/edit`,
        {
            email,
            firstName,
            lastName,
            description,
        },
        {
            Authorization: "Bearer " + jwt,
        }
    );

    if (!response.ok) {
        throw new Error(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE);
    }
    return await response.json();
};

export const editUserPasswоrd = async (email, jwt, oldPassword, newPassword) => {
    let response = await request.patch(
        `${WEB_API_BASE_URL}/users/user/edit/password`,
        {
            email,
            oldPassword: await sha512(oldPassword),
            newPassword: await sha512(newPassword),
        },
        {
            Authorization: "Bearer " + jwt,
        }
    );

    if (!response.ok) {
        throw new Error(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE);
    } else if (!response.ok) {
        throw new Error(INVALID_OLD_PASSWORD_PROVIDED_ERROR_MESSAGE);
    }

    return;
};

export const editUserImage = async (email, jwt, fileName, image) => {
    let response = await request.patch(
        `${WEB_API_BASE_URL}/users/user/edit/profileImage`,
        {
            email,
            fileName,
            image,
        },
        {
            Authorization: "Bearer " + jwt,
        }
    );

    if (!response.ok) {
        throw new Error(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE);
    }

    return await response.json();
};


export const editUserRole = async (jwt, manipulatedMemberId, roleToAssign) => {
    let response = await request.patch(
        `${WEB_API_BASE_URL}/organizations/change-member-role`,
        {
            manipulatedMemberId,
            roleToAssign,
        },
        {
            Authorization: "Bearer " + jwt,
        }
    );

    if (!response.ok) {
        throw new Error(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE);
    }

    return await response.text();
};

export const getUserDataByJWT = async (jwt) => {
    let response = await request.get(`${WEB_API_BASE_URL}/users/user`, null, {
        Authorization: "Bearer " + jwt,
    });

    if (!response.ok) {
        throw new Error(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE);
    }

    return await response.json();
};

export const deleteUserData = async (jwt, email) => {
    let response = await request.delete(`${WEB_API_BASE_URL}/users/${email}/delete`, null, {
        Authorization: "Bearer " + jwt,
    });

    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.json();
};

export const deleteUserImage = async (jwt, email) => {
    let response = await request.delete(`${WEB_API_BASE_URL}/users/${email}/image/delete`, null, {
        Authorization: "Bearer " + jwt,
    });

    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.json();
};

export const getUserDataByEmail = async (email) => {
    let response = await request.get(`${WEB_API_BASE_URL}/users/user/${email}`);

    return await response.json();
};

export const getUserEmailFromJWT = async (jwt) => {
    let response = await request.get(`${WEB_API_BASE_URL}/users/user/email`, null, {
        Authorization: "Bearer " + jwt,
    });

    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.text();
};

export const getUserGuidFromJWT = async (jwt) => {
    let response = await request.get(`${WEB_API_BASE_URL}/users/user/guid`, null, {
        Authorization: "Bearer " + jwt,
    });

    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.json();
};

export const getUsersBySearchTerm = (searchTerm, page, pageCount) => {
    return request
        .get(`${WEB_API_BASE_URL}/search/users/${searchTerm}?page=${page}&pageCount=${pageCount}`)
        .then((response) => response.json());
};

export const  getAuthenticatedUserOrganization = async (jwt, userEmail) => {
    var response = await request.get(`${WEB_API_BASE_URL}/users/${userEmail}/organization`, null,
    {
        Authorization: "Bearer " + jwt,
    });
    return await response.text();
} ;
