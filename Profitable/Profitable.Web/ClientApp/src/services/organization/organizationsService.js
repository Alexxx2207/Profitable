import { request } from "../../utils/fetch/request";
import {
    WEB_API_BASE_URL,
} from "../../common/config";

export const createOrganization = async (jwt, name) => {
    return await request.post(`${WEB_API_BASE_URL}/organizations/add-organization`, {
        name,
    }, {
        Authorization: "Bearer " + jwt,
    });
}

export const updateOrganization = async (jwt, name, organizationId) => {
    return await request.patch(`${WEB_API_BASE_URL}/organizations/update`, {
        name,
        organizationIdToUpdate: organizationId
    }, {
        Authorization: "Bearer " + jwt,
    });
}

export const addMemberFromOrganization = async (jwt, organizationId, memberId) => {
    return await request.patch(`${WEB_API_BASE_URL}/organizations/add-member`, {
        organizationId,
        memberId
    }, {
        Authorization: "Bearer " + jwt,
    });
}

export const removeMemberFromOrganization = async (jwt, memberToRemoveId) => {

    return await request.patch(`${WEB_API_BASE_URL}/organizations/remove-member`, {
        memberToRemoveId
    }, {
        Authorization: "Bearer " + jwt,
    });
}

export const getOrganization = async (organizationId) => {
    var response = await request.get(`${WEB_API_BASE_URL}/organizations/${organizationId}/get-organization`);

    return await response.json();
}

export const getOrganizationMessages = async (jwt, organizationId, page, pageCount) => {
    var response = await request.get(`${WEB_API_BASE_URL}/organizations/${organizationId}/get-messages?page=${page}&pageCount=${pageCount}`, null, {
        Authorization: "Bearer " + jwt,
    });

    if(!response.status === 400)
    {
        throw new Error(await response.text());
    }

    return await response.json();
}

export const addOrganizationMessage = async (jwt, organizationId, message) => {
    var response = await request.post(`${WEB_API_BASE_URL}/organizations/${organizationId}/add-message`, {
        organizationId,
        message
    }, {
        Authorization: "Bearer " + jwt,
    });

    if(!response.status === 400)
    {
        throw new Error(await response.text());
    }

    return await response.json();
}


export const deleteOrganizationMessage = async (jwt, organizationId) => {
    var response = await request.delete(
        `${WEB_API_BASE_URL}/organizations/delete?organizationId=${organizationId}`, 
        null, {
            Authorization: "Bearer " + jwt,
        }
    );

    if(!response.ok)
    {
        throw new Error(await response.text());
    }

    return response;
}