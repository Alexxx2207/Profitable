import { WEB_API_BASE_URL } from "../../common/config";
import { request } from "../../utils/fetch/request";

export const getUserJournals = async (jwt, page, pageCount) => {
    var response = await request.get(`${WEB_API_BASE_URL}/journals/get?page=${page}&pageCount=${pageCount}`,
        null,
        {
            Authorization: "Bearer " + jwt,
        });

    if (!response.ok) {
        throw new Error(await response.text());
    }
    

    return await response.json();
}

export const getSpecificUserJournals = async (jwt, journalId) => {
    var response = await request.get(`${WEB_API_BASE_URL}/journals/get/${journalId}`,
        null,
        {
            Authorization: "Bearer " + jwt,
        });

    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.json();
}

export const addUserJournals = async (jwt, title, content) => {
    var response = await request.post(`${WEB_API_BASE_URL}/journals/add`,
        {
            title,
            content
        },
        {
            Authorization: "Bearer " + jwt,
        });

    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.json();
}

export const updateUserJournals = async (jwt, journalId, title, content) => {
    var response = await request.patch(`${WEB_API_BASE_URL}/journals/update`,
        {
            journalId,
            title,
            content
        },
        {
            Authorization: "Bearer " + jwt,
        });

    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.json();
}

export const deleteUserJournals = async (jwt, journalId) => {
    var response = await request.delete(`${WEB_API_BASE_URL}/journals/delete?journalId=${journalId}`,
        null,
        {
            Authorization: "Bearer " + jwt,
        });

    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.json();
}