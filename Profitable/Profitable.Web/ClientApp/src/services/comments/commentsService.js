import { WEB_API_BASE_URL } from "../../common/config";
import { request } from "../../utils/fetch/request";

export const getCommentsByPostId = (postId) => {
    
    return request
        .get(`${WEB_API_BASE_URL}/comments/${postId}/all`)
        .then((response) => response.json());
};
