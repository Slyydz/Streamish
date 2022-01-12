import React from "react";
import "./App.css";
import VideoList from "./components/VideoList";
import { InputCards } from "./components/Search";
import { VideoForm } from "./components/VideoForm";

function App() {
  return (
    <div className="App">
      <InputCards />
      <br></br>
      <VideoForm />
    </div>
  );
}

export default App;