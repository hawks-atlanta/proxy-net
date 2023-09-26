const fs = require("fs");

fs.readFile("package.json", "utf8", (err, data) => {
  if (err) {
    console.error("Failed to read package.json:", err);
    process.exit(1);
  }
  const pkg = JSON.parse(data);
  console.log(`version=${pkg.version}`);
});
