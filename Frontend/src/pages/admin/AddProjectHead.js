import React, { useState, useEffect } from "react";
import axios from "axios";

export default function AddProjectHead() {
  const [projectHeadData, setProjectHeadData] = useState({
    name: "",
    userName: "",
    email: "",
    phoneNumber: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setProjectHeadData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        `${process.env.REACT_APP_API_URL}/ProjectHead/Register`,
        projectHeadData
      );
      alert(response.data);
      setProjectHeadData({
        name: "",
        userName: "",
        email: "",
        phoneNumber: "",
      });
    } catch (error) {
      console.error("Error registering ProjectHead:", error.message);
    }
  };
  return (
    <>
      <div class="align-middle inline-block min-w-full shadow overflow-hidden bg-white shadow-dashboard px-8 py-3 pb-8 rounded-bl-lg rounded-br-lg">
        <form onSubmit={handleSubmit} className="max-w-sm mx-auto mt-8">
          <div class="mb-6">
            <label
              for="name"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Name
            </label>
            <input
              type="text"
              id="name"
              name="name"
              value={projectHeadData.name}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <div class="mb-6">
            <label
              for="email"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Email
            </label>
            <input
              type="text"
              id="email"
              name="email"
              value={projectHeadData.email}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <div class="mb-6">
            <label
              for="userName"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              UserName
            </label>
            <input
              type="text"
              id="userName"
              name="userName"
              value={projectHeadData.userName}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <div class="mb-6">
            <label
              for="phoneNumber"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Phone Number
            </label>
            <input
              type="text"
              id="phoneNumber"
              name="phoneNumber"
              value={projectHeadData.phoneNumber}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <button
            type="submit"
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          >
            Create Project Head
          </button>
        </form>
      </div>
    </>
  );
}
