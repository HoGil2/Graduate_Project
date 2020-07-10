import cv2
import sys
import numpy as np
from tensorflow.compat.v2.keras.models import model_from_json

if len(sys.argv) != 4:

    sys.exit()

point1 = sys.argv[1]
point2 = sys.argv[2]
point3 = sys.argv[3]

img = np.zeros((32, 64), dtype=np.uint8)
images = np.zeros((1, 32, 64, 3), dtype=np.uint8)

point1 = point1[1:-1].split(',')
point2 = point2[1:-1].split(',')
point3 = point3[1:-1].split(',')

cv2.circle(img, (int(point1[0]), int(point1[1])), 1, (255, 255, 255), -1)
cv2.circle(img, (int(point2[0]), int(point2[1])), 1, (0, 0, 255), -1)
cv2.circle(img, (int(point3[0]), int(point3[1])), 1, (0, 0, 255), -1)

json_file = open("strike_guide_model.json", "r")
loaded_model_json = json_file.read()
json_file.close()

loaded_model = model_from_json(loaded_model_json)

loaded_model.load_weights("strike_guide_model.h5")

prob = np.max(loaded_model.predict(img)[0])
label = loaded_model.predict_classes(img)

print(label[0], round(prob,2))
