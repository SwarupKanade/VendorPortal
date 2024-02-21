function App() {
  return (
    <>
      <div className="min-h-screen flex items-center justify-center bg-gray-100">
        <div className="text-center">
          <h1 className="text-4xl font-bold text-gray-800 mb-4">
            Welcome to Student Management
          </h1>
          <p className="text-lg text-gray-600 mb-8">
            Explore and enjoy our content.
          </p>
          <a
            className="rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 mt-4"
            href="/login"
          >
            Login
          </a>
        </div>
      </div>
    </>
  );
}

export default App;
