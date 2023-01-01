import { useEffect } from "react";
import { Routes, Route, useLocation } from "react-router-dom";
import { AuthContextProvider } from "../../contexts/AuthContext";
import { MessageBoxContextProvider } from "../../contexts/MessageBoxContext";
import { NavBar } from "../Common/NavBar/NavBar";
import { Home } from "../Home/Home";
import { MarketsPage } from "../Markets/MarketsPage/MarketsPage";
import { Login } from "../Authentication/Login/Login";
import { Register } from "../Authentication/Register/Register";
import { Logout } from "../Authentication/Logout";
import { NotFoundPage } from "../Common/NotFoundPage/NotFoundPage";
import { Calcualtors } from "../Calculators/Calculators";
import { Calendars } from "../Calendars/Calendars";
import { NewsList } from "../News/NewsList";
import { NewsArticle } from "../News/NewsArticle/NewsArticle";
import { ProfilePage } from "../Users/ProfilePage/ProfilePage";
import { ProfileInfo } from "../Users/ProfileInfo/ProfileInfo";
import { AccountStatistics } from "../Users/AccountStatistics/AccountStatistics";
import { AddPositionsRecord } from "../Users/AccountStatistics/PositionsRecords/AddPositionsRecord/AddPositionsRecord";
import { FuturesRecordDetails } from "../Users/AccountStatistics/FuturesPositions/FuturesRecordDetails/FuturesRecordDetails";
import { SearchPage } from "../Search/SearchPage";
import { ChangePositionsRecord } from "../Users/AccountStatistics/PositionsRecords/ChangePositionsRecord/ChangePositionsRecord";
import { CreateFuturesPosition } from "../Users/AccountStatistics/FuturesPositions/CreateFuturesPosition/CreateFuturesPosition";
import { ChangeFuturesPosition } from "../Users/AccountStatistics/FuturesPositions/ChangeFuturesPosition/ChangeFuturesPosition";
import { StocksRecordDetails } from "../Users/AccountStatistics/StocksPositions/StocksRecordDetails/StocksRecordDetails";

// eslint-disable-next-line
import { CreateStocksPosition } from "../Users/AccountStatistics/StocksPositions/CreateStocksPosition/CreateStocksPosition";
import { ChangeStocksPosition } from "../Users/AccountStatistics/StocksPositions/ChangeStocksPosition/ChangeStocksPosition";
import { TimeContextProvider } from "../../contexts/TimeContext";

import { AddPositionToRecord } from "../Users/AccountStatistics/PositionsRecords/AddPositionToRecord/AddPositionToRecord";
import { FundamentalAnalysisPage } from "../FundamentalAnalysis/FundamentalAnalysisPage";
import { EducationPage } from "../Education/EducationPage";
import { AddOrganization } from "../Organizations/AddOrganization/AddOrganization";

import styles from "./App.module.css";
import { OrganizationPage } from "../Organizations/OrganizationPage/OrganizationPage";
import { ManageOrganization } from "../Organizations/ManageOrganization/ManageOrganization";

export function App() {
    const location = useLocation();

    useEffect(() => {
        window.scrollTo(0, 0);
    }, [location.pathname]);

    return (
        <AuthContextProvider location={location}>
            <MessageBoxContextProvider>
                <TimeContextProvider>
                    <div className={styles.content}>
                        <div className={styles.navigation}>
                            <NavBar />
                        </div>
                        <div className={styles.page}>
                            <Routes>
                                <Route path="/" element={<Home />} />

                                <Route path="/markets" element={<MarketsPage />} />

                                <Route path="/login" element={<Login />} />

                                <Route path="/register" element={<Register />} />

                                <Route path="/logout" element={<Logout />} />

                                <Route
                                    path="/users/:searchedProfileEmail"
                                    element={<ProfilePage />}
                                >
                                    <Route index element={<ProfileInfo />} />
                                    <Route path="personal-info" element={<ProfileInfo />} />
                                    <Route
                                        path="account-statistics"
                                        element={<AccountStatistics />}
                                    />
                                </Route>

                                <Route
                                    path="/users/:searchedProfileEmail/positions-records/create"
                                    element={<AddPositionsRecord />}
                                />

                                <Route
                                    path="/users/:searchedProfileEmail/positions-records/:recordId/change"
                                    element={<ChangePositionsRecord />}
                                />

                                <Route
                                    path="/users/:searchedProfileEmail/positions-records/futures/:recordGuid"
                                    element={<FuturesRecordDetails />}
                                />

                                <Route path="/positions/save" element={<AddPositionToRecord />} />

                                <Route
                                    path="/users/:searchedProfileEmail/positions-records/futures/:recordGuid/create-position"
                                    element={<CreateFuturesPosition />}
                                />

                                <Route
                                    path="/users/:searchedProfileEmail/positions-records/futures/:recordGuid/change-position/:positionGuid"
                                    element={<ChangeFuturesPosition />}
                                />

                                <Route
                                    path="/users/:searchedProfileEmail/positions-records/stocks/:recordGuid"
                                    element={<StocksRecordDetails />}
                                />

                                <Route
                                    path="/users/:searchedProfileEmail/positions-records/stocks/:recordGuid/create-position"
                                    element={<CreateStocksPosition />}
                                />

                                <Route
                                    path="/users/:searchedProfileEmail/positions-records/stocks/:recordGuid/change-position/:positionGuid"
                                    element={<ChangeStocksPosition />}
                                />

                                <Route path="/calculators" element={<Calcualtors />} />

                                <Route path="/news" element={<NewsList />} />

                                <Route path="/news/:newsTitle" element={<NewsArticle />} />

                                <Route path="/calendars" element={<Calendars />} />

                                <Route path="/search" element={<SearchPage />} />

                                <Route path="/organization" element={<OrganizationPage />} />

                                <Route path=":searchedProfileEmail/organizations/add" element={<AddOrganization />} />

                                <Route path="/organizations/:organizationId/manage" element={<ManageOrganization />} />

                                <Route 
                                    className={styles.fa_nav_link}
                                    path="/fundamental-analysis" 
                                    element={<FundamentalAnalysisPage />} />

                                <Route path="/education" element={<EducationPage />} />

                                <Route path="*" element={<NotFoundPage />} />
                            </Routes>
                        </div>
                    </div>
                </TimeContextProvider>
            </MessageBoxContextProvider>
        </AuthContextProvider>
    );
}
