import React, { useState, useEffect } from "react";
import axios from "axios";

export default function AddRFP() {
  const [rfpData, setRFPData] = useState({
    title: "",
    projectId: "",
    endDate: "",
    vendorCategoryId: "",
    documentFile: null,
  });
  const [vendorCategory, setVendorCategory] = useState([]);
  const [project, setProject] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const vcresponse = await axios.get(
          `${process.env.REACT_APP_API_URL}/VendorCategory/All`
        );

        setVendorCategory(vcresponse.data);

        const presponse = await axios.get(
          `${process.env.REACT_APP_API_URL}/Project/All`
        );

        setProject(presponse.data);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setRFPData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    setRFPData((prevData) => ({
      ...prevData,
      documentFile: file,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const formData = new FormData();
    formData.append("Title", rfpData.title);
    formData.append("DocumentFile", rfpData.documentFile);
    formData.append("ProjectId", rfpData.projectId);
    formData.append("VendorCategoryId", rfpData.vendorCategoryId);
    formData.append("EndDate", rfpData.endDate);
    try {
      const response = await axios.post(
        `${process.env.REACT_APP_API_URL}/RFP/Add`,
        formData
      );
      if (response.status === 200) alert("RFP Added");
      setRFPData({
        title: "",
        projectId: "",
        endDate: "",
        vendorCategoryId: "",
        documentFile: null,
      });
    } catch (error) {
      console.error("Error adding RFP:", error.message);
    }
  };
  return (
    <>
      <div class="align-middle inline-block min-w-full shadow overflow-hidden bg-white shadow-dashboard px-8 py-3 pb-8 rounded-bl-lg rounded-br-lg">
        <form onSubmit={handleSubmit} className="max-w-sm mx-auto mt-8">
          <div class="mb-6">
            <label
              for="title"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Title
            </label>
            <input
              type="text"
              id="title"
              name="title"
              value={rfpData.title}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <div class="mb-6">
            <label
              for="vendorCategoryId"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Select a Vendor Category
            </label>
            <select
              id="vendorCategoryId"
              name="vendorCategoryId"
              value={rfpData.vendorCategoryId}
              onChange={handleChange}
              className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            >
              <option value="" disabled>
                Choose a Category
              </option>
              {vendorCategory.map((item) => (
                <option key={item.id} value={item.id}>
                  {item.name}
                </option>
              ))}
            </select>
          </div>

          <div class="mb-6">
            <label
              for="projectId"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Select a Project
            </label>
            <select
              id="projectId"
              name="projectId"
              value={rfpData.projectId}
              onChange={handleChange}
              className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            >
              <option value="" disabled>
                Choose a Category
              </option>
              {project.map((item) => (
                <option key={item.id} value={item.id}>
                  {item.name}
                </option>
              ))}
            </select>
          </div>

          <div class="mb-6">
            <label
              for="endDate"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              End Date
            </label>
            <input
              type="datetime-local"
              id="endDate"
              name="endDate"
              value={rfpData.endDate}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <div class="mb-6">
            <label
              class="block mb-2 text-sm font-medium text-gray-900"
              for="documentFile"
            >
              Upload file
            </label>
            <input
              class="block w-full p-3 text-sm text-gray-900 border border-gray-300 rounded-lg cursor-pointer bg-gray-50 dark:text-gray-400 focus:outline-none"
              aria-describedby="file_input_help"
              id="documentFile"
              type="file"
              name="documentFile"
              accept=".png, .jpg, .pdf"
              onChange={handleFileChange}
            />
            <p class="mt-1 text-sm text-gray-500" id="file_input_help">
              PNG, JPG or PDF.
            </p>
          </div>

          <button
            type="submit"
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          >
            Add RFP
          </button>
        </form>
      </div>
    </>
  );
}
