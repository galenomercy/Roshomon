#Roshomon
# Roshomon
DeadlyHotel罗生门版
在一张地图上发生的凶杀和推理游戏简化版。


1、凶杀环节（普通）
①以如上4X4地图为例，总计16个格子，分成红黄绿蓝四个区域。
②点击开始凶杀按钮后，凶杀环节开始，UI界面消失。
③有ABCD和受害者五个角色分别用五个形状表示，初始化随机位置（可重叠）。随机挑选ABCD中一个人为凶手，其他三人为平民。
④每回合（代表从0点到23点）ABCD和受害者同时行动，随机向一个方向移动一格（可以重叠），并分别记录下来当前回合ABCD和受害者处于红黄绿蓝哪个区域，每个角色只能目击到处于同一颜色区域的其他玩家（包括自己）。
⑤当受害者和凶手处于同一颜色区域时，下一回合有50%几率凶手消耗一回合停留在原地击杀受害者，记录下此时的死亡时间。之后凶手依然可以随机移动，并且头像变黑，而受害者不再可以移动，并且头像变红。
⑥24回合后（代表0点到23点）凶杀环节结束，此时UI界面出现按钮“开始推理”。
⑦凶杀环节部分在后台运行，每个角色的初始位置和每回合移动对于玩家侦探而言不可见（也就是说侦探只能看到一张地图），直到最后揭晓谜底时才会展示。
2、推理环节（普通）

①进入推理环节，首先在[0,23]中随机选择一个长度为6的连续数组，包含凶杀环节中的死亡时间在其中，作为推理环节中的时间区间。例如死亡时间为16点，则可能返回数组[14,19]。打印一段话“法医鉴定，死亡时间在14点到19点之间。作为侦探，你要还原当时的案发场景，找出凶手，揭开真相。”
②推理环节的左边是询问界面。
每一轮询问，都可以先选择ABCD四人中任意一名角色进行，选择的那名角色头像变大。此时侦探可以就时间、人物、地点进行选择，然后点击询问，选择的那名角色会根据凶杀环节的记录，给予是/否/不知道三种答复。
例如：侦探选择A进行询问，从时间[14,19]，人物[A\B\C\D\受害者]，地点[红\黄\绿\蓝]中选择了“15点、B、红色区域”，则询问文案变成“在15点时，B在红色区域吗？”此时A会读取第16回合（15点）自己的记录进行判断并返回答案。判断的逻辑举例如下：①A在红色区域，没目击到B，则会回答“否”；②A在红色区域，目击到B，则会回答“是”；③A不在红色区域，目击到B，则会回答“否”；④A不在红色区域，没目击到B，则会回答“不知道”。（关于自己的询问则遵循②③两条，因为自己一定目击到自己。）
得到答复后，本轮不能再询问这名角色，其头像变灰，此时可以点击其他角色进行询问，但是之前问过的时间、人物、地点不能再选择。例如此时点击B询问，不能询问15点、B和红色区域。
四名角色全部询问完毕后，可以点击进入下一轮，此时询问对象和询问内容的限制重置。
③推理环节的右边是笔记界面，供玩家自主记录信息。通过滑动下方的滚动条，在每个时间点，都有一张包含四个颜色区域的空白地图以及ABCD受害者五名角色的头像。玩家根据询问信息和自己的判断，可以左键点击或者拖拽每个时间点的每个人物到地图界面上，从而记录一个有时间的可视化人物路线图。也可以右键点击或者拖拽出去角色头像到默认位置，还原自己的错误判断。
③玩家侦探可以在任意轮次点击推理完毕，出现提交界面，选择并提交自己关于时间、地点、人物的推理答案，也可以返回推理环节继续推理。



④提交后出现结算界面，此时会根据正确答案显示玩家侦探推理的正确与否，用√×在每个答案下方显示。下方显示评分，每推理正确一个答案+10分，每消耗一轮询问-2分，最终得分相加。

⑤点击查看答案，会显示弹出对话框显示正确的时间、地点、人物。

⑥点击查看凶杀环节，会揭晓谜底，将第一部分凶杀环节之前隐藏在后台的部分展示给玩家，按一定速率播放每个角色的行动路线以及凶手的杀人过程。

3、凶杀环节（高级）
高级凶杀环节增加了角色的说谎机制，体现不同身份之间的罗生门，增加了游戏的推理难度。
界面不变，在原先的基础上，按表格中的不同比例随机给ABCD四种角色赋予一个性格（六选一），仅仅在接受审讯时，会根据其性格将原先供词转化为另一种供词。

如上方所示，角色有10%几率是欺诈者，其表现为，当询问时本来应该回答“是”的场合，他回答“否”，本来应该回答“否”的场合，他回答“是”，回答“不知道”时不变。
4、推理环节（高级）

①侦探环节在原先基础上，左边的询问界面下面多了一个性格参考表，与第三部分的表格一致。
②右边的笔记界面上，增加了关于性格的标签，玩家可以像拖拽角色头像一样，将这些性格表情也拖拽在上方每个时间点的地图上进行记录。
③点击查看答案时，额外展示每个人的性格。
