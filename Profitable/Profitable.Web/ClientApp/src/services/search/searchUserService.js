import { WEB_API_BASE_URL } from "../../common/config";
import { request } from "../../utils/fetch/request";

export const getUsersBySearchTerm = (searchTerm, page, pageCount) => {
    return request
        .get(`${WEB_API_BASE_URL}/search/users/${searchTerm}?page=${page}&pageCount=${pageCount}`)
        .then((response) => response.json());
};
