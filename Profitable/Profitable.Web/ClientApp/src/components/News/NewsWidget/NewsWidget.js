import { useNavigate } from "react-router-dom";
import { ARTICLE_LOCAL_STORAGE_KEY } from "../../../common/config";
import { setLocalStorage } from "../../../utils/localStorage";

import styles from "./NewsWidget.module.css";

export const NewsWidget = ({ article }) => {
    const navigate = useNavigate();

    const openNews = () => {
        let selection = window.getSelection().toString();
        if (selection.length <= 0) {
            setLocalStorage(ARTICLE_LOCAL_STORAGE_KEY, article);
            navigate(`/news/${encodeURIComponent(article.title)}`);
        }
    };

    return (
        <div className={styles.newsWidget} onClick={(e) => openNews()}>
            <img src={article.image} alt="/" className={styles.newsImage} />
            <div className={styles.textDiv}>
                <h3 className={styles.title}>{article.title}</h3>
                <h6 className={styles.authorAndTime}>
                    {article.sender} {article.postedAgo ? "-" : ""} {article.postedAgo}
                </h6>
                <p className={styles.articleOverview}>{article.articleOverview}</p>
            </div>
        </div>
    );
};
