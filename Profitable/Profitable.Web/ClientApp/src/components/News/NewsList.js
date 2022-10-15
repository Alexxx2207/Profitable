import { useEffect, useRef, useState } from "react";
import { getAllNews } from "../../services/news/newsService";
import { NewsWidget } from "./NewsWidget/NewsWidget";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowDown } from "@fortawesome/free-solid-svg-icons";
import styles from "./NewsList.module.css";
import { GoToTop } from "../GoToTop/GoToTop";

export const NewsList = () => {
    const [news, setNews] = useState([]);

    const myRef = useRef(null);

    useEffect(() => {
      getAllNews().then((newNews) => setNews(newNews));
    }, []);

    const clickScrollDown = () => {
      window.scrollTo(0, myRef.current.getBoundingClientRect().y);
    };


    return (
      <div className={styles.newsListPageContainer}>
        <div className={styles.header}>
          <div className={styles.headerBlur}>
            <div className={styles.headerText}>
              <h1 className={styles.newsPageHeading}>News</h1>
              <h4 className={styles.subheading}>
                See them on the{" "}
                <span className={styles.goSpan} onClick={() => clickScrollDown()}>
                  GO
                </span>
              </h4>
            </div>
            <div className={styles.newsDirectionSign}>
              <div
                className={styles.arrowsContainer}
                onClick={() => clickScrollDown()}
              >
                <FontAwesomeIcon
                  icon={faArrowDown}
                  className={styles.arrowDownIcon1}
                  onClick={() => clickScrollDown()}
                />
                <FontAwesomeIcon
                  icon={faArrowDown}
                  className={styles.arrowDownIcon2}
                  onClick={() => clickScrollDown()}
                />
                <FontAwesomeIcon
                  icon={faArrowDown}
                  className={styles.arrowDownIcon3}
                  onClick={() => clickScrollDown()}
                />
                <FontAwesomeIcon
                  icon={faArrowDown}
                  className={styles.arrowDownIcon4}
                  onClick={() => clickScrollDown()}
                />
              </div>
            </div>
          </div>
        </div>
        <div className={styles.newsListContainer} ref={myRef}>
          <h1 className={styles.latestNews}>Latest News</h1>

          {news.length > 0 ? 
            news.map((newsArticle, index) => (
              <NewsWidget key={index} article={newsArticle} />
            ))
          :
              <h2 className={styles.notAvailableNews}>News are not available. Check your Internet Connection.</h2>
        }
        </div>
        <GoToTop />
      </div>
    );
};
