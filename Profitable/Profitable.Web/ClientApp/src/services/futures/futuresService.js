import { WEB_API_BASE_URL } from '../../common/config';
import { request } from '../../utils/fetch/request';

export const loadFuturesContracts = () => {
    return request.get(`${WEB_API_BASE_URL}/futures/all`)
    .then(response => response.json());
}