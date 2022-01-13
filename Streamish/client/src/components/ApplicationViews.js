import React from "react";
import { Switch, Route, Redirect } from "react-router-dom";
import VideoList from "./VideoList";
import { VideoForm } from "./VideoForm";
import VideoDetails from "./VideoDetails";
import Login from "./Login";
import { register } from "../modules/AuthManager";
import Register from "./Register";

const ApplicationViews = ({ isLoggedIn }) => {
    return (
        <Switch>
            <Route path="/" exact>

                {isLoggedIn ? <VideoList /> : <Redirect to="/login" />}

            </Route>

            <Route path="/videos/add">
                {isLoggedIn ? <VideoForm /> : <Redirect to="/login" />}
            </Route>

            <Route path="/videos/:id">
                {isLoggedIn ? <VideoDetails useparams /> : <Redirect to="/login" />}
            </Route>

            <Route path="/login">
                <Login />
            </Route>

            <Route path="/register">
                <Register />
            </Route>
        </Switch>
    );
};

export default ApplicationViews;