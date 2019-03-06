create or replace procedure get_partition_eager(resultItems out nocopy sys_refcursor) 
IS
begin
    open resultItems for
    select
        forum_partition.id as partition_id,
        forum_partition.name as partition_name,
        forum_category.id as category_id,
        forum_category.name as category_name,
        forum_category.description as category_description,
        COUNT(topic.id) as topic_count
    from topic
    right join forum_category on forum_category.id = topic.category_id
    right join forum_partition on forum_category.partition_id = forum_partition.id
    GROUP BY forum_partition.id, forum_partition.name, forum_category.id, forum_category.name, forum_category.description;
end;