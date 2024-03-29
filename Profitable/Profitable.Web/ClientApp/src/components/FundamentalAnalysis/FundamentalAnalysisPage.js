import { useLayoutEffect, useState } from "react";
import { COTReportsPage } from "./COTReports/COTReportsPage";
import { GoToTop } from "../Common/GoToTop/GoToTop";
import styles from "./FundamentalAnalysisPage.module.css";

export const FundamentalAnalysisPage = () => {
    const [windowWidth, setWindowWidth] = useState(0);
    
    const windowWidthSubstitude = 15;

    useLayoutEffect(() => {
      function updateWindowSize() {
        setWindowWidth(window.innerWidth);
      }

      window.addEventListener("resize", updateWindowSize);
      updateWindowSize();

      return () => window.removeEventListener("resize", updateWindowSize);
    }, []);
    
    return (
        <main className={styles.page}>
            <section className={styles.economicIndicatorsSection}>
                <h2 className={styles.economicIndicatorsHeader}>Economic Indicators</h2>
                <table className={styles.tableOfEconomicIndicators}>
                    <tbody>
                        <tr>
                            <td>
                                <iframe 
                                    title={"FFR 2YRS TB Yield"} 
                                    src={`https://fred.stlouisfed.org/graph/graph-landing.php?g=Yj5d&width=${windowWidth/2 - windowWidthSubstitude}&height=450`}
                                    className={styles.economicIndicatorStyle}
                                    loading="lazy"
                                ></iframe>
                            </td>
                            <td>
                                <iframe 
                                    title={"Yield Curve"} 
                                    src={`https://fred.stlouisfed.org/graph/graph-landing.php?g=YivB&width=${windowWidth/2 - windowWidthSubstitude}&height=450`}
                                    className={styles.economicIndicatorStyle}
                                    loading="lazy"
                                ></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <iframe 
                                    title={"Unemployment Rate"} 
                                    src={`https://fred.stlouisfed.org/graph/graph-landing.php?g=Xa3p&width=${windowWidth/2 - windowWidthSubstitude}&height=450`}
                                    className={styles.economicIndicatorStyle}
                                    loading="lazy"
                                ></iframe>
                            </td>
                            <td>
                                <iframe 
                                    title={"Balance Sheet: Total Assets / Wilshire"} 
                                    src={`https://fred.stlouisfed.org/graph/graph-landing.php?g=Xf2u&width=${windowWidth/2 - windowWidthSubstitude}&height=450`}
                                    className={styles.economicIndicatorStyle}
                                    loading="lazy"
                                ></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <iframe 
                                    title={"Federal Funds Effective Rate"} 
                                    src={`https://fred.stlouisfed.org/graph/graph-landing.php?g=XeZg&width=${windowWidth/2 - windowWidthSubstitude}&height=450`}
                                    className={styles.economicIndicatorStyle}
                                    loading="lazy"
                                ></iframe>
                            </td>
                            <td>
                                <iframe 
                                    title={"US Dollar To Other Currencies"} 
                                    src={`https://fred.stlouisfed.org/graph/graph-landing.php?g=Xf1u&width=${windowWidth/2 - windowWidthSubstitude}&height=450`}
                                    className={styles.economicIndicatorStyle}
                                    loading="lazy"
                                ></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <iframe 
                                    title={"Treasury Maturity Difference & Wilshire 5000"} 
                                    src={`https://fred.stlouisfed.org/graph/graph-landing.php?g=Xf01&width=${windowWidth/2 - windowWidthSubstitude}&height=450`}
                                    className={styles.economicIndicatorStyle}
                                    loading="lazy"
                                ></iframe>
                            </td>
                            <td>
                                <iframe 
                                    title={"Total Assets / S&P 500 Index"} 
                                    src={`https://fred.stlouisfed.org/graph/graph-landing.php?g=Xf0N&width=${windowWidth/2 - windowWidthSubstitude}&height=450`}
                                    className={styles.economicIndicatorStyle}
                                    loading="lazy"
                                ></iframe>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <iframe 
                    title={"Commercial Banks Activity"} 
                    src={`https://fred.stlouisfed.org/graph/graph-landing.php?g=Xf1a&width=${windowWidth - 45}&height=450`}
                    className={styles.economicIndicatorStyleFullWidth}
                    loading="lazy"
                ></iframe>
            </section>
            <section className={styles.COTSection}>
                <COTReportsPage />
            </section>

            <GoToTop />
        </main>
    );
}