import styles from './Post.module.css';

export const Post = (props) => {
    return (<div className={styles.post}>
        <div className={styles.content}>
            <h1>{props.title}</h1>
            <p>{props.content}</p>
        </div>
        <div className={styles.information}>
            <div className={styles.author}>
                {props.author}
            </div>
            <div className={styles.postedOn}>
                {props.postedOn}
            </div>
        </div>
        <hr style={{border: "1px solid white"}}/>
    </div>);
}