import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { ARTICLE_LOCAL_STORAGE_KEY } from "../../../common/config";
import { getAllNews, getNewsArticle } from "../../../services/news/newsService";
import {
  getFromLocalStorage,
  setLocalStorage,
} from "../../../utils/localStorage";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowCircleLeft } from "@fortawesome/free-solid-svg-icons";

import styles from "./NewsArticle.module.css";

export const NewsArticle = () => {

    const navigate = useNavigate();

    const { newsTitle } = useParams();

    const [article, setArticle] = useState({
        title: "",
        sender: "",
        postedAgo: "",
        articleText: "",
    });

    useEffect(() => {
        if (newsTitle === getFromLocalStorage(ARTICLE_LOCAL_STORAGE_KEY).title) {
        getNewsArticle(getFromLocalStorage(ARTICLE_LOCAL_STORAGE_KEY)).then(
            (newArticle) => setArticle(newArticle)
        );
        } else {
        getAllNews().then((allNews) => {
            getNewsArticle(allNews.find((news) => news.title === newsTitle)).then(
            (wholeArticle) => {
                setLocalStorage(ARTICLE_LOCAL_STORAGE_KEY, {
                    title: wholeArticle.title,
                    sender: wholeArticle.sender,
                    postedAgo: wholeArticle.postedAgo,
                    image: wholeArticle.image,
                    link: wholeArticle.link
                });
                setArticle(wholeArticle);
            }
            );
        });
        }
    }, []);

    const goBackButton = () => {
        navigate('/news');
    };

    return (
        <div className={styles.articleContainer}>
            <div className={styles.backButtonNewsArticle} onClick={(e) => goBackButton()}>
                <FontAwesomeIcon icon={faArrowCircleLeft} className={styles.backArrowButtonNewsArticle}/>
                <div className={styles.backText}>
                    Go Back
                </div>
            </div>
            <div className={styles.headerContainer}>
                <div className={styles.imageContainer} style={{
                    backgroundImage: `url("${article.image}")` 
                    }}>
                    <div className={styles.overlay}>
                        <h1 className={styles.title}>{article.title}</h1>
                    </div>
                </div>
            </div>
            <h6 className={styles.authorAndTime}>
                {article.sender} - {article.postedAgo}
            </h6>
            <div className={styles.articleText}>
                {article.articleText?.split("\\n").map((paragraph, index) => (
                <p key={index}>{paragraph}</p>
                ))}
            </div>
        </div>
    );
};
