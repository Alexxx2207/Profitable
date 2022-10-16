import { WEB_API_BASE_URL } from '../../common/config';
import { request } from '../../utils/fetch/request';

export const getUsersBySearchTerm = (searchTerm) => {
    return request.get(`${WEB_API_BASE_URL}/search/users/${searchTerm}`)
        .then(response => response.json());
}