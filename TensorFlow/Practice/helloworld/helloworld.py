import tensorflow as tf
import matplotlib.pyplot as plt
import random

from tensorflow.examples.tutorials.mnist import input_data
mnist = input_data.read_data_sets("MNIST_data/", one_hot=True)

# 학습의 속도와 Epoch, 그리고 데이터를 가져올 batch의 크기를 지정해 줍니다.
learning_rate = 0.001
training_epochs = 3
batch_size = 100

# 먼저 이미지의 정보를 담을 X를 placeholder로 지정해줍니다.
X = tf.placeholder(tf.float32, [None, 784])

# 가져온 정보를 이미지화 하기 위하여 reshape합니다.
X_img = tf.reshape(X, [-1, 28, 28, 1])   # img 28x28x1 (black/white)

# 이미지의 Label 정보를 담을 Y 역시 placeholder로 지정해줍니다.
Y = tf.placeholder(tf.float32, [None, 10])

# CNN에서 우리는 결국 제대로 된 Filter를 가진 Model을 구축해 나갈 것입니다.
# 다시 말해 Filter를 학습시켜 이미지를 제대로 인식하도록 할 것입니다.
# 그렇기에 Filter를 변수 W로 표현합니다.
# 32개의 3 x 3 x 1의 Filter를 사용하겠다는 뜻입니다.
W1 = tf.Variable(tf.random_normal([3,3,1,32], stddev=0.01))

# 간단하게 conv2d 함수를 사용하면 됩니다.
L1 = tf.nn.conv2d(X_img, W1, strides=[1,1,1,1], padding='SAME')
L1 = tf.nn.relu(L1)
L1 = tf.nn.max_pool(L1, ksize=[1,2,2,1], strides=[1,2,2,1], padding='SAME')

# W2는 두번째 Conv의 Filter입니다.
# 다만 이전 과정 Filter의 개수가 32개였기 때문에
# 그 숫자에 맞추어 depth를 32로 지정해줍니다.
W2 = tf.Variable(tf.random_normal([3,3,32,64], stddev=0.01))

L2 = tf.nn.conv2d(L1, W2, strides=[1,1,1,1], padding='SAME')
L2 = tf.nn.relu(L2)
L2 = tf.nn.max_pool(L2, ksize=[1,2,2,1], strides=[1,2,2,1], padding='SAME')

# Softmax를 통한 FC layer를 활용하기 위해 shape를 변환해줍니다.
# 위에서 L2는 일종의 X 값이라고도 볼 수 있습니다.
# Softmax를 거칠 예측값(WX+b)을 만들어주기 위해 reshape 합니다.

L2 = tf.reshape(L2, [-1, 7*7*64])

# W3를 설정하는데 Xavier initializing을 통해 초기값을 설정할 것입니다.
# reshape된 L2의 shape이 [None, 7*7*64] 였으므로
# W3의 shape은 [7*7*64, num_label]이 됩니다.
W3 = tf.get_variable("W3", shape=[7*7*64, 10], initializer = tf.contrib.layers.xavier_initializer())
b = tf.Variable(tf.random_normal([10]))

hypothesis = tf.matmul(L2, W3) + b

# Softmax 함수를 직접 사용하는 대신에 sofmax_corss_entropy_with_logits을 사용할 수 있습니다.
# 인자로 logits과 label을 전달해주면 됩니다.
cost = tf.reduce_mean(tf.nn.softmax_cross_entropy_with_logits(logits=hypothesis, labels=Y))

# 이전까지는 Gradient Descent Optimizer를 사용하였지만
# 좀 더 학습성과가 뛰어나다고 알려져있는 Adam Optimizer를 사용하겠습니다.
optimizer = tf.train.AdamOptimizer(learning_rate=learning_rate).minimize(cost)

sess = tf.Session()
sess.run(tf.global_variables_initializer())

print('Learning started. It takes sometime.')
for epoch in range(training_epochs):
    avg_cost = 0
    total_batch = int(mnist.train.num_examples / batch_size)

    for i in range(total_batch):
        batch_xs, batch_ys = mnist.train.next_batch(batch_size)
        feed_dict = {X: batch_xs, Y: batch_ys}
        c, _ = sess.run([cost, optimizer], feed_dict=feed_dict)
        avg_cost += c / total_batch

    print('Epoch:', '%04d' % (epoch + 1), 'cost =', '{:.9f}'.format(avg_cost))

print('Learning Finished!')

# 먼저 늘 그래왔듯 accuracy op를 만들고요.
correct_prediction = tf.equal(tf.argmax(hypothesis, 1), tf.argmax(Y, 1))
accuracy = tf.reduce_mean(tf.cast(correct_prediction, tf.float32))

# MNIST의 테스트용 이미지를 feed_dict로 전달합니다.
print('Accuracy:', sess.run(accuracy, feed_dict={X: mnist.test.images, Y: mnist.test.labels}))

# 랜덤하게 하나의 이미지를 정확히 맞추는지 역시 테스트가 가능합니다.
r = random.randint(0, mnist.test.num_examples - 1)
print("Label: ", sess.run(tf.argmax(mnist.test.labels[r:r + 1], 1)))
print("Prediction: ", sess.run(
    tf.argmax(hypothesis, 1), feed_dict={X: mnist.test.images[r:r + 1]}))

# matplotlib을 사용하여 랜덤하게 뽑힌 이미지를 출력할 수도 있습니다.
plt.imshow(mnist.test.images[r:r + 1].reshape(28, 28), cmap='Greys', interpolation='nearest')
plt.show()