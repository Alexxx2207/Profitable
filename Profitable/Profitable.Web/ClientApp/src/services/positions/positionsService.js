import { WEB_API_BASE_URL } from "../../common/config"
import { request } from "../../utils/fetch/request"


export const getUserPositions = async (email, page, pageCount, orderPositionsRecordBy) => {
    var response = await request.post(`${WEB_API_BASE_URL}/positions/records/byuser`, {
        userEmail: email,
        page,
        pageCount,
        orderPositionsRecordBy
    });

    if(response.status === 400) {
        throw new Error('Invalid request');
    }

    return await response.json();
}