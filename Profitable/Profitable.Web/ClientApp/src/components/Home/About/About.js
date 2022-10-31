import styles from "./About.module.css";

export const About = () => {
    return (
        <div>
            <h1 className={styles.aboutUsHeading}>About Us</h1>
            <div className={styles.aboutContainer}>
                <section className={styles.aboutSection}>
                    <h1 className={styles.sectionHeading}>Who We Are</h1>

                    <p className={styles.aboutParagraph}>
                        We are Profitable - a large company, aiming to help institutional and retail
                        traders achieve more by making their everyday job easier. We believe that
                        when a person is connected to many others and has access to instant
                        information, he/she is one step closer to professionalism.
                    </p>
                </section>

                <section className={styles.aboutSection}>
                    <h1 className={styles.sectionHeading}>What We Do</h1>
                    <p className={styles.aboutParagraph}>
                        We develop and deliver a software platform which makes exchanging
                        information, receiving news and communication immediate, secure and
                        reliable.
                    </p>
                </section>

                <section className={styles.aboutSection}>
                    <h1 className={styles.sectionHeading}>Contact Us</h1>

                    <p className={styles.aboutParagraph}>
                        Office Location:
                        <br />
                        <a href="https://g.page/GaritagePark?share">Geritage Park Sofia</a>
                    </p>
                    <p className={styles.aboutParagraph}>Email: customer.support@profitable.com</p>
                </section>
            </div>
        </div>
    );
};
