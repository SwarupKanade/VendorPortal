import React, { useState, useEffect } from "react";
import axios from "axios";

export default function AddProject() {
  const [projectData, setProjectData] = useState({
    name: "",
    projectHeadId: "",
    projectStatus: "",
    description: "",
  });
  const [projectHead, setProjectHead] = useState([]);

  useEffect(() => {
    const fetchProjectHead = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/ProjectHead/All`
        );

        setProjectHead(response.data);
      } catch (error) {
        console.error("Error fetching ProjectHead data:", error);
      }
    };

    fetchProjectHead();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setProjectData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        `${process.env.REACT_APP_API_URL}/Project/Create`,
        projectData
      );
      if (response.status === 200) alert("Project Added");
      setProjectData({
        name: "",
        projectHeadId: "",
        projectStatus: "",
        description: "",
      });
    } catch (error) {
      console.error("Error adding VendorCategory:", error.message);
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
              value={projectData.name}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <div class="mb-6">
            <label
              for="projectHeadId"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Select an option
            </label>
            <select
              id="projectHeadId"
              name="projectHeadId"
              value={projectData.projectHeadId}
              onChange={handleChange}
              className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            >
              <option value="" disabled>
                Choose a Project Head
              </option>
              {projectHead.map((item) => (
                <option key={item.id} value={item.id}>
                  {item.name}
                </option>
              ))}
            </select>
          </div>
          <div class="mb-6">
            <div class="flex">
              <div class="flex items-center me-4">
                <input
                  id="radio-1"
                  type="radio"
                  value="Active"
                  name="projectStatus"
                  onChange={handleChange}
                  class="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 focus:ring-blue-500"
                />
                <label
                  for="radio-1"
                  class="ms-2 text-sm font-medium text-gray-900"
                >
                  Active
                </label>
              </div>
              <div class="flex items-center me-4">
                <input
                  id="radio-2"
                  type="radio"
                  value="Ongoing"
                  name="projectStatus"
                  onChange={handleChange}
                  class="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 focus:ring-blue-500 "
                />
                <label
                  for="radio-2"
                  class="ms-2 text-sm font-medium text-gray-900"
                >
                  Ongoing
                </label>
              </div>
              <div class="flex items-center me-4">
                <input
                  id="radio-3"
                  type="radio"
                  value="Done"
                  name="projectStatus"
                  onChange={handleChange}
                  class="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 focus:ring-blue-500 "
                />
                <label
                  for="radio-3"
                  class="ms-2 text-sm font-medium text-gray-900"
                >
                  Done
                </label>
              </div>
            </div>
          </div>

          <div class="mb-6">
            <label
              for="description"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Description
            </label>
            <input
              type="text"
              id="description"
              name="description"
              value={projectData.description}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <button
            type="submit"
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          >
            Add Project
          </button>
        </form>
      </div>
    </>
  );
}
