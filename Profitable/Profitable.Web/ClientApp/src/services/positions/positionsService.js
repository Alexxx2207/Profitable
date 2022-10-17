import { WEB_API_BASE_URL } from "../../common/config"
import { request } from "../../utils/fetch/request"


export const getPositionsOrderByOptions = async () => {
    var response = await request.get(`${WEB_API_BASE_URL}/positions/records/order-options`);

    if(response.status === 400) {
        throw new Error('Invalid request');
    }

    return await response.json();
}

export const getUserPositions = (email, page, pageCount, orderPositionsRecordBy) => {
    return request
    .post(`${WEB_API_BASE_URL}/positions/records/by-user`, {
        userEmail: email,
        page,
        pageCount,
        orderPositionsRecordBy
    })
    .then((response) => response.json());;
}

export const deleteUserPositionsRecord = (JWT, recordGuid) => {
    return request
    .delete(`${WEB_API_BASE_URL}/positions/records/delete/${recordGuid}`, 
    null,
    {
        'Authorization': 'Bearer ' +  JWT
    })
    .then((response) => response.json());;
}

export const createPositionsRecord = async (JWT, email, recordName, instrumentGroup) => {
    var response = await request.post(`${WEB_API_BASE_URL}/positions/records/create`, {
        userEmail: email,
        recordName,
        instrumentGroup
    },
    {
        'Authorization': 'Bearer ' +  JWT
    });

    if(response.status === 400) {
        throw new Error(await response.text());
    }

    return;
}

export const changePositionsRecord = async (JWT, recordGuid, recordName) => {
    var response = await request.patch(`${WEB_API_BASE_URL}/positions/records/change/${recordGuid}`, {
        recordName,
    },
    {
        'Authorization': 'Bearer ' +  JWT
    });

    if(response.status === 400) {
        throw new Error(await response.text());
    }

    return;
}