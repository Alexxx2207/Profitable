import { BookListItem } from "./BookListItem/BookListItem";
import styles from "./BooksList.module.css";

export const BookList = ({books}) => {
    return (
        <div className={styles.bookListContainer}>
            {books.map(book => <BookListItem key={book.guid} book={book} /> )}
        </div>
    );
}