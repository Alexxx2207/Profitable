import styles from "./BookListItem.module.css";

export const BookListItem = ({book}) => {
    return (
        <div className={styles.widgetContainer}>
            <h2 className={styles.bookTitle}>
                Title: {book.title}
            </h2>
            <h5 className={styles.bookAuthors}>
                Authors: {book.authors}
            </h5>
        </div>
    );
}