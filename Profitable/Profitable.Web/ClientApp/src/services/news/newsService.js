import { WEB_API_BASE_URL } from "../../common/config";
import { request } from "../../utils/fetch/request";

export const getAllNews = () => {
    return request.get(WEB_API_BASE_URL + "/news/allNewsOverview").then((res) => res.json());
};

export const getNewsArticle = (article) => {
    return request
        .post(WEB_API_BASE_URL + "/news/newsArticle", {
            image: article.image,
            title: article.title,
            sender: article.sender,
            postedAgo: article.postedAgo,
            link: article.link,
        })
        .then((res) => res.json());
};
